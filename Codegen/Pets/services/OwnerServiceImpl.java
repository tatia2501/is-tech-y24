package com.lab2.Pets.services;

import com.lab2.Pets.entities.Owner;
import com.lab2.Pets.repositories.OwnerRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
public class OwnerServiceImpl implements OwnerService {
    private final OwnerRepository ownerRepository;

    @Autowired
    public OwnerServiceImpl(OwnerRepository ownerRepository, PetService petService) {
        this.ownerRepository = ownerRepository;
    }

    public void create(Owner owner) {
        this.ownerRepository.save(owner);
    }

    public Optional<Owner> findById(long id) {
        return this.ownerRepository.findById(id);
    }
}