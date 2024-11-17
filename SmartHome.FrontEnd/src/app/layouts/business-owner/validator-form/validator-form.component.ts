import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { BusinessService } from '../../../../backend/services/Business/business.service';

@Component({
  selector: 'app-validator-form',
  templateUrl: './validator-form.component.html',
  styleUrl: './validator-form.component.css'
})
export class ValidatorFormComponent {
  selectedValidator: string | null = null;

  readonly formField: any = {
    validatorId: {
      name: "validatorId",
    }
  }

  readonly validatorForm = new FormGroup({
    [this.formField.validatorId.name]: new FormControl("", [])
  });

  constructor(
    private readonly _businessService: BusinessService
  ) {}

  onValidatorChange(value: string) {
    this.selectedValidator = value;
    this.validatorForm.patchValue({
      [this.formField.validatorId.name]: value
    });
  }

  public onSubmit() {
    if (this.validatorForm.invalid) {
      return;
    }
    const validatorId = this.validatorForm.get(this.formField.validatorId.name)?.value;
    if (!validatorId) {
      return;
    }

    this._businessService.addValidatorToBusiness(validatorId).subscribe({
      next: () => {
        this.validatorForm.reset();
        this.selectedValidator = null;
      },
      error: () => {
        console.error("Error setting validator");
      }
    });
  }
}
