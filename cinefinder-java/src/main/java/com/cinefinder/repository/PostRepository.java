package com.cinefinder.repository;

import com.cinefinder.model.Post;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.domain.Pageable;
import java.util.List;

public interface PostRepository extends JpaRepository<Post, Long> {
    List<Post> findByUsuarioId(Long usuarioId);
    List<Post> findAllByOrderByDataCriacaoDesc(Pageable pageable);
}
