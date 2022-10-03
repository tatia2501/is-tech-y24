package com.lab2.Pets.controllers;

import com.lab2.Pets.entities.Owner;
import com.lab2.Pets.services.OwnerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Optional;

@RestController
@RequestMapping
public class OwnerController {
    @Autowired
    private OwnerService ownerService;

    @Autowired
    public OwnerController(OwnerService ownerService) {
        this.ownerService = ownerService;
    }

    @GetMapping(value = "getOwner", produces = MediaType.APPLICATION_JSON_VALUE)
    public Optional<Owner> getOwner(long id) {
        return ownerService.findById(id);
    }

    @PostMapping(value = "createOwner", produces = MediaType.APPLICATION_JSON_VALUE)
    public ResponseEntity<?> create(@RequestBody Owner owner) {
        ownerService.create(owner);
        return new ResponseEntity<>(HttpStatus.CREATED);
    }
}