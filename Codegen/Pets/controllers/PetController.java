package com.lab2.Pets.controllers;

import com.lab2.Pets.entities.Pet;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.lab2.Pets.services.PetService;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping
public class PetController {
    @Autowired
    private PetService petService;

    @Autowired
    public PetController(PetService petService) {
        this.petService = petService;
    }

    @GetMapping(value = "getPet", produces = MediaType.APPLICATION_JSON_VALUE)
    public Optional<Pet> getPet(long id) {
        return petService.findById(id);
    }

    @GetMapping(value = "getAllPets", produces = MediaType.APPLICATION_JSON_VALUE)
    public List<Pet> getAllPets() {
        return petService.readAll();
    }

    @GetMapping(value = "getAnimalByName", produces = MediaType.APPLICATION_JSON_VALUE)
    public List<Pet> findByNameAndAnimal(String animal, String name) {
        return petService.findByNameAndAnimal(animal, name);
    }

    @PostMapping(value = "createPet", produces = MediaType.APPLICATION_JSON_VALUE)
    public ResponseEntity<?> create(@RequestBody Pet pet) {
        petService.create(pet);
        return new ResponseEntity<>(HttpStatus.CREATED);
    }
}
