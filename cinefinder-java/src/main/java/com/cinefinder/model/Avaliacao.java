package com.cinefinder.model;

import jakarta.persistence.*;
import lombok.*;
import java.time.LocalDateTime;

@Entity @Table(name = "avaliacoes")
@Getter @Setter @NoArgsConstructor @AllArgsConstructor @Builder
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

    @PrePersist
    void prePersist() {
        if (dataCriacao == null) dataCriacao = LocalDateTime.now();
    }
}
