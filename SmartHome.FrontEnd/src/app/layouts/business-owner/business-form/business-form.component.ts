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
  selectedValidator: string | null = null;

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
    },
    validatorId: {
      name: "validatorId",
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
    ]),
    [this.formField.validatorId.name]: new FormControl("", [])
  });

  businessStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _businessService: BusinessService
  ){}

  onValidatorChange(value: string) {
    this.selectedValidator = value;
    this.businessForm.patchValue({
      [this.formField.validatorId.name]: value
    });
  }

  public goToValidator(){
    this._router.navigate(["/businessOwners/updateValidator"]);
  }
  public onSubmit(values : BusinessCreationModel) {
    this.businessStatus = { loading: true };

    const businessData: BusinessCreationModel = {
      name: values.name,
      rut: values.rut,
      logo: values.logo,
      validatorId: this.businessForm.get(this.formField.validatorId.name)?.value
    };

    this._businessService.registerBusiness(businessData).subscribe({
      next: (response) => {
        this.businessStatus = null;

        this._router.navigate(["/landing"]);
      },
      error: (error) => {
        this.businessStatus = { error };
      },
    });
  }
}
