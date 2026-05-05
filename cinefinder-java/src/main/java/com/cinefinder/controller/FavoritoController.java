package com.cinefinder.controller;

import com.cinefinder.model.*;
import com.cinefinder.repository.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/favoritos")
public class FavoritoController {

    private final FavoritoRepository repo;
    private final UsuarioRepository usuarioRepo;

    public FavoritoController(FavoritoRepository repo, UsuarioRepository usuarioRepo) {
        this.repo = repo;
        this.usuarioRepo = usuarioRepo;
    }

    @GetMapping
    public List<Map<String,Object>> listar() {
        return repo.findAll().stream().map(this::toMap).toList();
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String,Object>> buscar(@PathVariable Long id) {
        return repo.findById(id).map(f -> ResponseEntity.ok(toMap(f)))
                .orElse(ResponseEntity.notFound().build());
    }

    @GetMapping("/usuario/{usuarioId}")
    public List<Map<String,Object>> porUsuario(@PathVariable Long usuarioId) {
        return repo.findByUsuarioId(usuarioId).stream().map(this::toMap).toList();
    }

    @PostMapping
    public ResponseEntity<Map<String,Object>> criar(@RequestBody Map<String,Object> body) {
        Favorito f = new Favorito();
        Long uid = ((Number) body.get("usuarioId")).longValue();
        usuarioRepo.findById(uid).ifPresent(f::setUsuario);
        if (body.get("filmeId") != null)
            f.setFilmeId(((Number) body.get("filmeId")).longValue());
        if (body.get("serieId") != null)
            f.setSerieId(((Number) body.get("serieId")).longValue());
        return ResponseEntity.status(HttpStatus.CREATED).body(toMap(repo.save(f)));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deletar(@PathVariable Long id) {
        if (!repo.existsById(id)) return ResponseEntity.notFound().build();
        repo.deleteById(id);
        return ResponseEntity.noContent().build();
    }

    private Map<String,Object> toMap(Favorito f) {
        Map<String,Object> m = new LinkedHashMap<>();
        m.put("id", f.getId());
        m.put("usuarioId", f.getUsuario() != null ? f.getUsuario().getId() : null);
        m.put("usuarioNickname", f.getUsuario() != null ? f.getUsuario().getNickname() : null);
        m.put("filmeId", f.getFilmeId());
        m.put("serieId", f.getSerieId());
        m.put("dataAdicionado", f.getDataAdicionado() != null ? f.getDataAdicionado().toString() : null);
        return m;
    }
}
