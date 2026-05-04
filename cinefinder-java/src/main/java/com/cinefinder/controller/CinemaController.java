package com.cinefinder.controller;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.client.RestTemplate;

import java.net.URLEncoder;
import java.nio.charset.StandardCharsets;
import java.util.*;

/**
 * API de Cinemas Próximos — agrega dados do OpenStreetMap (Overpass API)
 * e Nominatim (geocoding), sem chave de API necessária.
 *
 * Endpoints:
 *   GET /api/cinemas/proximos?lat=-23.5505&lng=-46.6333&raio=5
 *   GET /api/cinemas/cidade?q=São+Paulo&raio=5
 */
@RestController
@RequestMapping("/api/cinemas")
public class CinemaController {

    private static final String OVERPASS_URL = "https://overpass-api.de/api/interpreter";
    private static final String NOMINATIM_URL = "https://nominatim.openstreetmap.org/search";

    private final RestTemplate http = new RestTemplate();
    private final ObjectMapper mapper = new ObjectMapper();

    // ── GET /api/cinemas/proximos ────────────────────────────────────
    @GetMapping("/proximos")
    public ResponseEntity<Map<String, Object>> proximos(
            @RequestParam double lat,
            @RequestParam double lng,
            @RequestParam(defaultValue = "5") double raio) {

        int metros = (int) (raio * 1000);
        List<Map<String, Object>> cinemas = buscarOverpass(lat, lng, metros);

        Map<String, Object> resp = new LinkedHashMap<>();
        resp.put("total", cinemas.size());
        resp.put("lat", lat);
        resp.put("lng", lng);
        resp.put("raioKm", raio);
        resp.put("fonte", "OpenStreetMap / Overpass API");
        resp.put("cinemas", cinemas);
        return ResponseEntity.ok(resp);
    }

    // ── GET /api/cinemas/cidade ──────────────────────────────────────
    @GetMapping("/cidade")
    public ResponseEntity<?> porCidade(
            @RequestParam String q,
            @RequestParam(defaultValue = "5") double raio) {

        // 1. Geocodificar cidade via Nominatim
        double[] coords = geocodificar(q);
        if (coords == null) {
            return ResponseEntity.badRequest()
                    .body(Map.of("erro", "Cidade não encontrada: " + q));
        }

        int metros = (int) (raio * 1000);
        List<Map<String, Object>> cinemas = buscarOverpass(coords[0], coords[1], metros);

        Map<String, Object> resp = new LinkedHashMap<>();
        resp.put("cidade", q);
        resp.put("lat", coords[0]);
        resp.put("lng", coords[1]);
        resp.put("raioKm", raio);
        resp.put("total", cinemas.size());
        resp.put("fonte", "OpenStreetMap / Overpass API");
        resp.put("cinemas", cinemas);
        return ResponseEntity.ok(resp);
    }

    // ── Helpers ──────────────────────────────────────────────────────

    private List<Map<String, Object>> buscarOverpass(double lat, double lng, int metros) {
        // Overpass QL: busca nodes e ways com amenity=cinema no raio dado
        String query = String.format(
                "[out:json][timeout:20];" +
                "(node[\"amenity\"=\"cinema\"](around:%d,%.6f,%.6f);" +
                " way[\"amenity\"=\"cinema\"](around:%d,%.6f,%.6f););" +
                "out center tags;",
                metros, lat, lng, metros, lat, lng);

        List<Map<String, Object>> result = new ArrayList<>();
        try {
            String url = OVERPASS_URL + "?data=" + URLEncoder.encode(query, StandardCharsets.UTF_8);
            HttpHeaders headers = new HttpHeaders();
            headers.set("User-Agent", "CineFinder/1.0 (educational project)");
            HttpEntity<Void> req = new HttpEntity<>(headers);

            ResponseEntity<String> resp = http.exchange(url, HttpMethod.GET, req, String.class);
            JsonNode root = mapper.readTree(resp.getBody());
            JsonNode elements = root.path("elements");

            for (JsonNode el : elements) {
                Map<String, Object> cinema = new LinkedHashMap<>();
                JsonNode tags = el.path("tags");

                // Coordenadas: node tem lat/lon direto; way tem center
                double elLat = el.has("lat") ? el.get("lat").asDouble()
                        : el.path("center").path("lat").asDouble();
                double elLng = el.has("lon") ? el.get("lon").asDouble()
                        : el.path("center").path("lon").asDouble();

                cinema.put("id", el.path("id").asLong());
                cinema.put("nome", tags.has("name") ? tags.get("name").asText() : "Cinema sem nome");
                cinema.put("lat", elLat);
                cinema.put("lng", elLng);
                cinema.put("endereco", montarEndereco(tags));
                cinema.put("telefone", tags.has("phone") ? tags.get("phone").asText() : null);
                cinema.put("site", tags.has("website") ? tags.get("website").asText() : null);
                cinema.put("capacidade", tags.has("capacity") ? tags.get("capacity").asText() : null);
                cinema.put("wheelchair", tags.has("wheelchair") ? tags.get("wheelchair").asText() : null);
                cinema.put("openingHours", tags.has("opening_hours") ? tags.get("opening_hours").asText() : null);
                cinema.put("distanciaKm", calcularDistancia(lat, lng, elLat, elLng));
                cinema.put("mapUrl", String.format(
                        "https://www.openstreetmap.org/?mlat=%.6f&mlon=%.6f#map=17/%.6f/%.6f",
                        elLat, elLng, elLat, elLng));
                cinema.put("googleMapsUrl", String.format(
                        "https://www.google.com/maps/search/?api=1&query=%.6f,%.6f", elLat, elLng));

                result.add(cinema);
            }

            // Ordena por distância
            result.sort(Comparator.comparingDouble(c -> ((Number) c.get("distanciaKm")).doubleValue()));

        } catch (Exception e) {
            // Retorna lista vazia em caso de falha da Overpass API
        }
        return result;
    }

    private double[] geocodificar(String cidade) {
        try {
            String url = NOMINATIM_URL + "?q=" +
                    URLEncoder.encode(cidade, StandardCharsets.UTF_8) +
                    "&format=json&limit=1";
            HttpHeaders headers = new HttpHeaders();
            headers.set("User-Agent", "CineFinder/1.0 (educational project)");
            HttpEntity<Void> req = new HttpEntity<>(headers);

            ResponseEntity<String> resp = http.exchange(url, HttpMethod.GET, req, String.class);
            JsonNode root = mapper.readTree(resp.getBody());
            if (root.isArray() && root.size() > 0) {
                JsonNode first = root.get(0);
                return new double[]{
                        first.get("lat").asDouble(),
                        first.get("lon").asDouble()
                };
            }
        } catch (Exception ignored) {}
        return null;
    }

    private String montarEndereco(JsonNode tags) {
        List<String> partes = new ArrayList<>();
        if (tags.has("addr:street")) {
            String rua = tags.get("addr:street").asText();
            if (tags.has("addr:housenumber")) rua += ", " + tags.get("addr:housenumber").asText();
            partes.add(rua);
        }
        if (tags.has("addr:suburb"))  partes.add(tags.get("addr:suburb").asText());
        if (tags.has("addr:city"))    partes.add(tags.get("addr:city").asText());
        if (tags.has("addr:state"))   partes.add(tags.get("addr:state").asText());
        return partes.isEmpty() ? null : String.join(", ", partes);
    }

    /** Fórmula de Haversine para distância entre dois pontos em km */
    private double calcularDistancia(double lat1, double lng1, double lat2, double lng2) {
        final double R = 6371.0;
        double dLat = Math.toRadians(lat2 - lat1);
        double dLng = Math.toRadians(lng2 - lng1);
        double a = Math.sin(dLat / 2) * Math.sin(dLat / 2)
                + Math.cos(Math.toRadians(lat1)) * Math.cos(Math.toRadians(lat2))
                * Math.sin(dLng / 2) * Math.sin(dLng / 2);
        double dist = R * 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        return Math.round(dist * 100.0) / 100.0;
    }
}
