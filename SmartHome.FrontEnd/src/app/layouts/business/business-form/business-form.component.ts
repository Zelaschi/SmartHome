import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import BusinessCreationModel from '../../../../backend/services/Business/models/BusinessCreationModel';
import { BusinessService } from '../../../../backend/services/Business/business.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-business-form',
  templateUrl: './business-form.component.html',
  styleUrl: './business-form.component.css'
})

export class BusinessFormComponent {
  readonly formField: any = {
    name: {
      name: "name",
      required: "Name is required"
    },
    rut: {
      name: "rut",
      required: "Rut is required"
    },
    logo: {
      name: "logo",
      required: "Logo is required"
    }
  };
  readonly businessForm = new FormGroup({
    [this.formField.name.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.rut.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.logo.name]: new FormControl("", [
      Validators.required
    ])
  });

  businessStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _businessService: BusinessService
  ){}

  public onSubmit(values : BusinessCreationModel) {
    this.businessStatus = { loading: true };

    this._businessService.registerBusiness(values).subscribe({
      next: (response) => {
        this.businessStatus = null;

        this._router.navigate(["/devices"]);
      },
      error: (error) => {
        this.businessStatus = { error };
      },
    });
  }
}
