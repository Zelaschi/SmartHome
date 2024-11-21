import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AdminService } from '../../../../backend/services/Admin/admin.service';
import AdminCreationModel from '../../../../backend/services/Admin/models/AdminCreationModel';

@Component({
  selector: 'app-admin-form',
  templateUrl: './admin-form.component.html',
  styleUrl: './admin-form.component.css'
})
export class AdminFormComponent {
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
  readonly adminForm = new FormGroup({
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

  adminStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _adminService: AdminService
  ){}

  public onSubmit(values : AdminCreationModel) {
    this.adminStatus = { loading: true };

    this._adminService.registerAdmin(values).subscribe({
      next: (response) => {
        this.adminStatus = null;

        this._router.navigate(["/landing"]);
      },
      error: (error) => {
        this.adminStatus = { error: error };
      }
    });
  }
}
