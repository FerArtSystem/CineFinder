package com.cinefinder.model;

import jakarta.persistence.*;
import java.time.LocalDateTime;

@Entity @Table(name = "avaliacoes")
public class Avaliacao {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne(fetch = FetchType.LAZY) @JoinColumn(name = "usuario_id", nullable = false)
    private Usuario usuario;

    /** ID do filme na API C# / TMDB (referência externa, sem FK local) */
    @Column(name = "filme_id")
    private Long filmeId;

    /** ID da série na API C# / TMDB (referência externa, sem FK local) */
    @Column(name = "serie_id")
    private Long serieId;

    @Column(nullable = false)
    private Integer nota; // 1–10

    @Column(columnDefinition = "TEXT")
    private String comentario;

    @Column(nullable = false)
    private LocalDateTime dataCriacao;

    public Avaliacao() {
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

    public Long getFilmeId() {
        return filmeId;
    }

    public void setFilmeId(Long filmeId) {
        this.filmeId = filmeId;
    }

    public Long getSerieId() {
        return serieId;
    }

    public void setSerieId(Long serieId) {
        this.serieId = serieId;
    }

    public Integer getNota() {
        return nota;
    }

    public void setNota(Integer nota) {
        this.nota = nota;
    }

    public String getComentario() {
        return comentario;
    }

    public void setComentario(String comentario) {
        this.comentario = comentario;
    }

    public LocalDateTime getDataCriacao() {
        return dataCriacao;
    }

    public void setDataCriacao(LocalDateTime dataCriacao) {
        this.dataCriacao = dataCriacao;
    }

    @PrePersist
    void prePersist() {
        if (dataCriacao == null) dataCriacao = LocalDateTime.now();
    }
}
