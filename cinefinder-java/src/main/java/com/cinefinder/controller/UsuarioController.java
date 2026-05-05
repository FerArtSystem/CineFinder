package com.cinefinder.controller;

import com.cinefinder.model.Usuario;
import com.cinefinder.repository.UsuarioRepository;
import org.springframework.http.*;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.util.*;

@RestController
@RequestMapping("/api/usuarios")
public class UsuarioController {

    private final UsuarioRepository repo;
    private final PasswordEncoder encoder;

    public UsuarioController(UsuarioRepository repo, PasswordEncoder encoder) {
        this.repo = repo;
        this.encoder = encoder;
    }

    @GetMapping
    public List<Map<String,Object>> listar() {
        return repo.findAll().stream().map(this::toMap).toList();
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String,Object>> buscar(@PathVariable Long id) {
        return repo.findById(id).map(u -> ResponseEntity.ok(toMap(u)))
                .orElse(ResponseEntity.notFound().build());
    }

    @PostMapping
    public ResponseEntity<?> criar(@RequestBody Map<String,String> body) {
        if (repo.existsByEmail(body.get("email")))
            return ResponseEntity.badRequest().body(Map.of("message","E-mail já cadastrado"));
        if (repo.existsByNickname(body.get("nickname")))
            return ResponseEntity.badRequest().body(Map.of("message","Nickname já em uso"));

        Usuario u = new Usuario();
        u.setNome(body.get("nome"));
        u.setEmail(body.get("email"));
        u.setNickname(body.get("nickname"));
        u.setSenhaHash(encoder.encode(body.get("senha")));
        u.setDataCadastro(LocalDateTime.now());
        return ResponseEntity.status(HttpStatus.CREATED).body(toMap(repo.save(u)));
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> atualizar(@PathVariable Long id, @RequestBody Map<String,String> body) {
        return repo.findById(id).map(u -> {
            if (body.containsKey("nome")) u.setNome(body.get("nome"));
            if (body.containsKey("nickname")) u.setNickname(body.get("nickname"));
            if (body.containsKey("senha") && !body.get("senha").isBlank())
                u.setSenhaHash(encoder.encode(body.get("senha")));
            return ResponseEntity.ok(toMap(repo.save(u)));
        }).orElse(ResponseEntity.notFound().build());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deletar(@PathVariable Long id) {
        if (!repo.existsById(id)) return ResponseEntity.notFound().build();
        repo.deleteById(id);
        return ResponseEntity.noContent().build();
    }

    @PostMapping("/login")
    public ResponseEntity<?> login(@RequestBody Map<String,String> body) {
        return repo.findByEmail(body.get("email"))
                .filter(u -> encoder.matches(body.get("senha"), u.getSenhaHash()))
                .map(u -> ResponseEntity.ok(toMap(u)))
                .orElse(ResponseEntity.status(HttpStatus.UNAUTHORIZED)
                .body(Map.<String, Object>of("message", "Credenciais inválidas")));
    }

    private Map<String,Object> toMap(Usuario u) {
        return Map.of(
            "id", u.getId(),
            "nome", u.getNome(),
            "email", u.getEmail(),
            "nickname", u.getNickname(),
            "dataCadastro", u.getDataCadastro().toString()
        );
    }
}
