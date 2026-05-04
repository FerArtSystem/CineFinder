package com.cinefinder.repository;

import com.cinefinder.model.Avaliacao;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface AvaliacaoRepository extends JpaRepository<Avaliacao, Long> {
    List<Avaliacao> findByFilmeId(Long filmeId);
    List<Avaliacao> findBySerieId(Long serieId);
    List<Avaliacao> findByUsuarioId(Long usuarioId);
}
