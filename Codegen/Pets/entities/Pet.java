package com.lab2.Pets.entities;

import com.fasterxml.jackson.annotation.JsonBackReference;

import javax.persistence.*;

@Entity
@Table(name = "pets")
public class Pet {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    private long id;

    @Column(name = "name")
    private String name;

    @Column(name = "animal")
    private String animal;

    @Column(name = "birth")
    private String date;

    @ManyToOne
    @JsonBackReference
    @JoinColumn(name = "ownerID", nullable = false)
    private Owner owner;

    public String getName() {
        return name;
    }

    public void setName(String name){
        this.name = name;
    }

    public Long getId(){
        return id;
    }

    public void setId(Long id){
        this.id = id;
    }

    public String getAnimal() {
        return animal;
    }

    public void setAnimal(String animal){
        this.animal = animal;
    }

    public String getDate() {
        return date;
    }

    public void setDate(String date){
        this.date = date;
    }

    public Pet(){
    }

    public Pet(Long id, String name, String animal, String date){
        this.id = id;
        this.name = name;
        this.animal = animal;
        this.date = date;
    }
}