import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HomeService } from '../../../../backend/services/Home/home.service';

@Component({
  selector: 'app-home-device-name',
  templateUrl: './home-device-name.component.html',
  styleUrl: './home-device-name.component.css'
})
export class HomeDeviceNameComponent {
  @Input() homeDeviceId: string | null = null;
  @Output() homeDeviceNameUpdated = new EventEmitter<string>();

  readonly formField: any = {
    name: {
      name: "name",
      required: "Name is required"
    }
  };

  readonly homeDeviceNameForm = new FormGroup({
    [this.formField.name.name]: new FormControl("", [
      Validators.required
    ])
  });

  homeDeviceNameStatus:{
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private homeService: HomeService
  ) {}

  public onSubmit(): void {
    if (this.homeDeviceNameForm.invalid || !this.homeDeviceId) {
      return;
    }

    const newName = this.homeDeviceNameForm.get(this.formField.name.name)?.value;

    this.homeDeviceNameStatus = { loading: true };

    this.homeService.UpdateHomeDeviceName(this.homeDeviceId, newName).subscribe({
      next: () => {
        this.homeDeviceNameStatus = null;
        this.homeDeviceNameUpdated.emit(newName);
      },
      error: (error) => {
        this.homeDeviceNameStatus = { error: 'Failed to update home device name. Please try again.' };
        console.error(error);
      }
    });
  }
}
