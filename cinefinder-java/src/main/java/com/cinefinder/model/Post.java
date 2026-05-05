package com.cinefinder.model;

import jakarta.persistence.*;
import java.time.LocalDateTime;

@Entity @Table(name = "posts")
public class Post {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne(fetch = FetchType.LAZY) @JoinColumn(name = "usuario_id", nullable = false)
    private Usuario usuario;

    @Column(nullable = false, columnDefinition = "TEXT")
    private String conteudo;

    @Column(nullable = false)
    private LocalDateTime dataCriacao;

    private LocalDateTime dataEdicao;

    public Post() {
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Usuario getUsuario() {
        return usuario;
    }

    public void setUsuario(Usuario usuario) {
        this.usuario = usuario;
    }

    public String getConteudo() {
        return conteudo;
    }

    public void setConteudo(String conteudo) {
        this.conteudo = conteudo;
    }

    public LocalDateTime getDataCriacao() {
        return dataCriacao;
    }

    public void setDataCriacao(LocalDateTime dataCriacao) {
        this.dataCriacao = dataCriacao;
    }

    public LocalDateTime getDataEdicao() {
        return dataEdicao;
    }

    public void setDataEdicao(LocalDateTime dataEdicao) {
        this.dataEdicao = dataEdicao;
    }

    @PrePersist
    void prePersist() {
        if (dataCriacao == null) dataCriacao = LocalDateTime.now();
    }
}
