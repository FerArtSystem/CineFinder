package com.cinefinder.controller;

import com.cinefinder.model.*;
import com.cinefinder.repository.*;
import lombok.RequiredArgsConstructor;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.util.*;

@RestController
@RequestMapping("/api/avaliacoes")
@RequiredArgsConstructor
public class AvaliacaoController {

    private final AvaliacaoRepository repo;
    private final UsuarioRepository usuarioRepo;

    @GetMapping
    public List<Map<String,Object>> listar() {
        return repo.findAll().stream().map(this::toMap).toList();
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String,Object>> buscar(@PathVariable Long id) {
        return repo.findById(id).map(a -> ResponseEntity.ok(toMap(a)))
                .orElse(ResponseEntity.notFound().build());
    }

    @GetMapping("/filme/{filmeId}")
    public List<Map<String,Object>> porFilme(@PathVariable Long filmeId) {
        return repo.findByFilmeId(filmeId).stream().map(this::toMap).toList();
    }

    @GetMapping("/serie/{serieId}")
    public List<Map<String,Object>> porSerie(@PathVariable Long serieId) {
        return repo.findBySerieId(serieId).stream().map(this::toMap).toList();
    }

    @GetMapping("/usuario/{usuarioId}")
    public List<Map<String,Object>> porUsuario(@PathVariable Long usuarioId) {
        return repo.findByUsuarioId(usuarioId).stream().map(this::toMap).toList();
    }

    @PostMapping
    public ResponseEntity<?> criar(@RequestBody Map<String,Object> body) {
        Avaliacao a = new Avaliacao();
        a.setNota(((Number) body.get("nota")).intValue());
        a.setComentario((String) body.get("comentario"));
        a.setDataCriacao(LocalDateTime.now());

        Long uid = ((Number) body.get("usuarioId")).longValue();
        usuarioRepo.findById(uid).ifPresent(a::setUsuario);

        if (body.get("filmeId") != null)
            a.setFilmeId(((Number) body.get("filmeId")).longValue());
        if (body.get("serieId") != null)
            a.setSerieId(((Number) body.get("serieId")).longValue());

        return ResponseEntity.status(HttpStatus.CREATED).body(toMap(repo.save(a)));
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> atualizar(@PathVariable Long id, @RequestBody Map<String,Object> body) {
        return repo.findById(id).map(a -> {
            if (body.get("nota") != null) a.setNota(((Number) body.get("nota")).intValue());
            if (body.containsKey("comentario")) a.setComentario((String) body.get("comentario"));
            return ResponseEntity.ok(toMap(repo.save(a)));
        }).orElse(ResponseEntity.notFound().build());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deletar(@PathVariable Long id) {
        if (!repo.existsById(id)) return ResponseEntity.notFound().build();
        repo.deleteById(id);
        return ResponseEntity.noContent().build();
    }

    private Map<String,Object> toMap(Avaliacao a) {
        Map<String,Object> m = new LinkedHashMap<>();
        m.put("id", a.getId());
        m.put("usuarioId", a.getUsuario() != null ? a.getUsuario().getId() : null);
        m.put("usuarioNickname", a.getUsuario() != null ? a.getUsuario().getNickname() : null);
        m.put("filmeId", a.getFilmeId());
        m.put("serieId", a.getSerieId());
        m.put("nota", a.getNota());
        m.put("comentario", a.getComentario());
        m.put("dataCriacao", a.getDataCriacao() != null ? a.getDataCriacao().toString() : null);
        return m;
    }
}

    @GetMapping
    public List<Map<String,Object>> listar() {
        return repo.findAll().stream().map(this::toMap).toList();
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String,Object>> buscar(@PathVariable Long id) {
        return repo.findById(id).map(a -> ResponseEntity.ok(toMap(a)))
                .orElse(ResponseEntity.notFound().build());
    }

    @GetMapping("/filme/{filmeId}")
    public List<Map<String,Object>> porFilme(@PathVariable Long filmeId) {
        return repo.findByFilmeId(filmeId).stream().map(this::toMap).toList();
    }

    @GetMapping("/serie/{serieId}")
    public List<Map<String,Object>> porSerie(@PathVariable Long serieId) {
        return repo.findBySerieId(serieId).stream().map(this::toMap).toList();
    }

    @GetMapping("/usuario/{usuarioId}")
    public List<Map<String,Object>> porUsuario(@PathVariable Long usuarioId) {
        return repo.findByUsuarioId(usuarioId).stream().map(this::toMap).toList();
    }

    @PostMapping
    public ResponseEntity<?> criar(@RequestBody Map<String,Object> body) {
        Avaliacao a = new Avaliacao();
        a.setNota(((Number) body.get("nota")).intValue());
        a.setComentario((String) body.get("comentario"));
        a.setDataCriacao(LocalDateTime.now());

        Long uid = ((Number) body.get("usuarioId")).longValue();
        usuarioRepo.findById(uid).ifPresent(a::setUsuario);

        if (body.get("filmeId") != null) {
            filmeRepo.findById(((Number) body.get("filmeId")).longValue()).ifPresent(a::setFilme);
        }
        if (body.get("serieId") != null) {
            serieRepo.findById(((Number) body.get("serieId")).longValue()).ifPresent(a::setSerie);
        }

        return ResponseEntity.status(HttpStatus.CREATED).body(toMap(repo.save(a)));
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> atualizar(@PathVariable Long id, @RequestBody Map<String,Object> body) {
        return repo.findById(id).map(a -> {
            if (body.get("nota") != null) a.setNota(((Number) body.get("nota")).intValue());
            if (body.containsKey("comentario")) a.setComentario((String) body.get("comentario"));
            return ResponseEntity.ok(toMap(repo.save(a)));
        }).orElse(ResponseEntity.notFound().build());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deletar(@PathVariable Long id) {
        if (!repo.existsById(id)) return ResponseEntity.notFound().build();
        repo.deleteById(id);
        return ResponseEntity.noContent().build();
    }

    private Map<String,Object> toMap(Avaliacao a) {
        Map<String,Object> m = new LinkedHashMap<>();
        m.put("id", a.getId());
        m.put("usuarioId", a.getUsuario() != null ? a.getUsuario().getId() : null);
        m.put("usuarioNickname", a.getUsuario() != null ? a.getUsuario().getNickname() : null);
        m.put("filmeId", a.getFilme() != null ? a.getFilme().getId() : null);
        m.put("filmeTitulo", a.getFilme() != null ? a.getFilme().getTitulo() : null);
        m.put("serieId", a.getSerie() != null ? a.getSerie().getId() : null);
        m.put("serieTitulo", a.getSerie() != null ? a.getSerie().getTitulo() : null);
        m.put("nota", a.getNota());
        m.put("comentario", a.getComentario());
        m.put("dataCriacao", a.getDataCriacao() != null ? a.getDataCriacao().toString() : null);
        return m;
    }
}
