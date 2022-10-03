package com.lab2.Pets.services;

import com.lab2.Pets.entities.Pet;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import com.lab2.Pets.repositories.PetRepository;

import java.util.*;

@Service
public class PetServiceImpl implements PetService {
    private final PetRepository petRepository;

    @Autowired
    public PetServiceImpl(PetRepository petRepository){
        this.petRepository = petRepository;
    }

    public Optional<Pet> findById(Long id) {
        return petRepository.findById(id);
    }

    public void create(Pet pet){
        petRepository.save(pet);
    }

    public List<Pet> readAll() {
        return new ArrayList(this.petRepository.findAll());
    }

    public List<Pet> findByNameAndAnimal(String animal, String name){
        List<Pet> res = new List<Pet>() {
            @Override
            public int size() {
                return 0;
            }

            @Override
            public boolean isEmpty() {
                return false;
            }

            @Override
            public boolean contains(Object o) {
                return false;
            }

            @Override
            public Iterator<Pet> iterator() {
                return null;
            }

            @Override
            public Object[] toArray() {
                return new Object[0];
            }

            @Override
            public <T> T[] toArray(T[] a) {
                return null;
            }

            @Override
            public boolean add(Pet pet) {
                return false;
            }

            @Override
            public boolean remove(Object o) {
                return false;
            }

            @Override
            public boolean containsAll(Collection<?> c) {
                return false;
            }

            @Override
            public boolean addAll(Collection<? extends Pet> c) {
                return false;
            }

            @Override
            public boolean addAll(int index, Collection<? extends Pet> c) {
                return false;
            }

            @Override
            public boolean removeAll(Collection<?> c) {
                return false;
            }

            @Override
            public boolean retainAll(Collection<?> c) {
                return false;
            }

            @Override
            public void clear() {

            }

            @Override
            public Pet get(int index) {
                return null;
            }

            @Override
            public Pet set(int index, Pet element) {
                return null;
            }

            @Override
            public void add(int index, Pet element) {

            }

            @Override
            public Pet remove(int index) {
                return null;
            }

            @Override
            public int indexOf(Object o) {
                return 0;
            }

            @Override
            public int lastIndexOf(Object o) {
                return 0;
            }

            @Override
            public ListIterator<Pet> listIterator() {
                return null;
            }

            @Override
            public ListIterator<Pet> listIterator(int index) {
                return null;
            }

            @Override
            public List<Pet> subList(int fromIndex, int toIndex) {
                return null;
            }
        };
        var mem = petRepository.findAll();
        for (Pet m: mem) {
            if (m.getAnimal() == animal && m.getName() == name){
                res.add(m);
            }
        }
        return res;
    }
}
