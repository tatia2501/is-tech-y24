package com.lab2.Pets.services;

import com.lab2.Pets.entities.Pet;

import java.util.List;
import java.util.Optional;

public interface PetService {
    Optional<Pet> findById(Long id);
    void create(Pet pet);
    List<Pet> readAll();
    List<Pet> findByNameAndAnimal(String animal, String name);
}
