package com.lab2.Pets.repositories;

import com.lab2.Pets.entities.Pet;
import org.springframework.data.jpa.repository.JpaRepository;

public interface PetRepository extends JpaRepository<Pet, Long> {
}
