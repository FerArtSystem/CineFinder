package com.cinefinder.model;

import jakarta.persistence.*;
import java.time.LocalDateTime;

@Entity @Table(name = "favoritos")
public class Favorito {

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
    private LocalDateTime dataAdicionado;

    public Favorito() {
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

    public LocalDateTime getDataAdicionado() {
        return dataAdicionado;
    }

    public void setDataAdicionado(LocalDateTime dataAdicionado) {
        this.dataAdicionado = dataAdicionado;
    }

    @PrePersist
    void prePersist() {
        if (dataAdicionado == null) dataAdicionado = LocalDateTime.now();
    }
}
