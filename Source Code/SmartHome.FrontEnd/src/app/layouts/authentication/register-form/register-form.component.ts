import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HomeOwnerService } from '../../../../backend/services/HomeOwner/home-owner.service';
import HomeOwnerCreationModel from '../../../../backend/services/HomeOwner/models/HomeOwnerCreationModel';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.css'],
})
export class RegisterFormComponent {
  readonly formField: any = {
    name: {
      name: 'name',
      required: 'Name is required',
    },
    surname: {
      name: 'surname',
      required: 'Surname is required',
    },
    email: {
      name: 'email',
      required: 'email is required',
      email: 'email is not valid',
    },
    password: {
      name: 'password',
      required: 'password is required',
      minlength: 'password must be at least 6 characters long',
    },
    profilePhoto: {
      name: 'profilePhoto',
    },
  };

  readonly registerForm = new FormGroup({
    [this.formField.name.name]: new FormControl('', [
      Validators.required,
    ]),
    [this.formField.surname.name]: new FormControl('', [
      Validators.required,
    ]),
    [this.formField.email.name]: new FormControl('', [
      Validators.required,
      Validators.email,
    ]),
    [this.formField.password.name]: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
    ]),
    [this.formField.profilePhoto.name]: new FormControl('', []),
  });

  registerStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _homeOwnerService: HomeOwnerService
  ) {}

  public onSubmit(values: HomeOwnerCreationModel) {
    this.registerStatus = { loading: true };
    this._homeOwnerService.registerHomeOwner(values).subscribe({
      next: (response) => {
        this.registerStatus = null;

        this._router.navigate(['/login']);
      },
      error: (error) => {
        this.registerStatus = null;
        this.registerStatus = { error };
      },
    });
  }

}