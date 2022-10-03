package com.lab2.Pets.services;

import com.lab2.Pets.entities.Owner;
import java.util.Optional;

public interface OwnerService {
    void create(Owner owner);
    Optional<Owner> findById(long id);
}