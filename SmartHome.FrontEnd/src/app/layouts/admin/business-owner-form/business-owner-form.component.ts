import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BusinessOwnerService } from '../../../../backend/services/BusinessOwner/business-owner.service';
import BusinessOwnerCreationModel from '../../../../backend/services/BusinessOwner/models/BusinessOwnerCreationModel';

@Component({
  selector: 'app-business-owner-form',
  templateUrl: './business-owner-form.component.html',
  styleUrl: './business-owner-form.component.css'
})
export class BusinessOwnerFormComponent {
  readonly formField: any = {
    name: {
      name: "name",
      required: "Name is required"
    },
    surname: {
      name: "surname",
      required: "Surname is required"
    },
    email: {
      name: "email",
      required: "Email is required"
    },
    password: {
      name: "password",
      required: "Password is required"
    }
  };
  readonly businessOwnerForm = new FormGroup({
    [this.formField.name.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.surname.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.email.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.password.name]: new FormControl("", [
      Validators.required
    ])
  });

  businessOwnerStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _businessOwnerService: BusinessOwnerService
  ){}

  public onSubmit(values : BusinessOwnerCreationModel) {
    this.businessOwnerStatus = { loading: true };

    this._businessOwnerService.registerBusinessOwner(values).subscribe({
      next: (response) => {
        this.businessOwnerStatus = null;

        this._router.navigate(["/landing"]);
      },
      error: (error) => {
        this.businessOwnerStatus = { error: error };
      }
    });
  }
}
