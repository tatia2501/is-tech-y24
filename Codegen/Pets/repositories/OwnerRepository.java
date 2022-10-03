package com.lab2.Pets.repositories;

import com.lab2.Pets.entities.Owner;
import org.springframework.data.jpa.repository.JpaRepository;

public interface OwnerRepository extends JpaRepository<Owner, Long> {
}