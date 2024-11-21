import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HomeService } from '../../../../backend/services/Home/home.service';
import HomeCreationModel from '../../../../backend/services/Home/models/HomeCreationModel';

@Component({
  selector: 'app-home-form',
  templateUrl: './home-form.component.html',
  styleUrl: './home-form.component.css'
})
export class HomeFormComponent {
  readonly formField: any = {
    name: {
      name: "name",
      required: "Name is required"
    },
    mainStreet: {
      name: "mainStreet",
      required: "Main street is required"
    },
    doorNumber: {
      name: "doorNumber",
      required: "Door number is required"
    },
    latitude: {
      name: "latitude",
      required: "Latitude is required"
    },
    longitude: {
      name: "longitude",
      required: "Longitude is required"
    },
    maxMembers: {
      name: "maxMembers",
      required: "Max members is required"
    }
  };
  readonly homeForm = new FormGroup({
    [this.formField.name.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.mainStreet.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.doorNumber.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.latitude.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.longitude.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.maxMembers.name]: new FormControl("", [
      Validators.required
    ])
  });

  homeStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _homeService: HomeService
  ){}

  public onSubmit(values : HomeCreationModel) {
    this.homeStatus = { loading: true };

    this._homeService.registerHome(values).subscribe({
      next: (response) => {
        this.homeStatus = null;

        this._router.navigate(["/homeOwners"]);
      },
      error: (error) => {
        this.homeStatus = { error };
      },
    });
  }
}
