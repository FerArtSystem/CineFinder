package com.cinefinder.controller;

import com.cinefinder.model.*;
import com.cinefinder.repository.*;
import org.springframework.data.domain.PageRequest;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.util.*;

@RestController
@RequestMapping("/api/posts")
public class PostController {

    private final PostRepository repo;
    private final UsuarioRepository usuarioRepo;

    public PostController(PostRepository repo, UsuarioRepository usuarioRepo) {
        this.repo = repo;
        this.usuarioRepo = usuarioRepo;
    }

    @GetMapping
    public List<Map<String,Object>> listar() {
        return repo.findAllByOrderByDataCriacaoDesc(PageRequest.of(0, 50))
                .stream().map(this::toMap).toList();
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String,Object>> buscar(@PathVariable Long id) {
        return repo.findById(id).map(p -> ResponseEntity.ok(toMap(p)))
                .orElse(ResponseEntity.notFound().build());
    }

    @GetMapping("/usuario/{usuarioId}")
    public List<Map<String,Object>> porUsuario(@PathVariable Long usuarioId) {
        return repo.findByUsuarioId(usuarioId).stream().map(this::toMap).toList();
    }

    @PostMapping
    public ResponseEntity<Map<String,Object>> criar(@RequestBody Map<String,Object> body) {
        Post p = new Post();
        p.setConteudo((String) body.get("conteudo"));
        p.setDataCriacao(LocalDateTime.now());
        Long uid = ((Number) body.get("usuarioId")).longValue();
        usuarioRepo.findById(uid).ifPresent(p::setUsuario);
        return ResponseEntity.status(HttpStatus.CREATED).body(toMap(repo.save(p)));
    }

    @PutMapping("/{id}")
    public ResponseEntity<Map<String,Object>> atualizar(@PathVariable Long id, @RequestBody Map<String,Object> body) {
        return repo.findById(id).map(p -> {
            p.setConteudo((String) body.get("conteudo"));
            p.setDataEdicao(LocalDateTime.now());
            return ResponseEntity.ok(toMap(repo.save(p)));
        }).orElse(ResponseEntity.notFound().build());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deletar(@PathVariable Long id) {
        if (!repo.existsById(id)) return ResponseEntity.notFound().build();
        repo.deleteById(id);
        return ResponseEntity.noContent().build();
    }

    private Map<String,Object> toMap(Post p) {
        Map<String,Object> m = new LinkedHashMap<>();
        m.put("id", p.getId());
        m.put("usuarioId", p.getUsuario() != null ? p.getUsuario().getId() : null);
        m.put("usuarioNickname", p.getUsuario() != null ? p.getUsuario().getNickname() : null);
        m.put("conteudo", p.getConteudo());
        m.put("dataCriacao", p.getDataCriacao() != null ? p.getDataCriacao().toString() : null);
        m.put("dataEdicao", p.getDataEdicao() != null ? p.getDataEdicao().toString() : null);
        return m;
    }
}
