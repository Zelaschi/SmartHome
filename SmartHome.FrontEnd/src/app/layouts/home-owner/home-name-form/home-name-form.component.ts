import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HomeService } from '../../../../backend/services/Home/home.service';

@Component({
  selector: 'app-home-name-form',
  templateUrl: './home-name-form.component.html',
  styleUrl: './home-name-form.component.css'
})
export class HomeNameFormComponent {
  @Input() homeId: string | null = null;
  @Output() nameUpdated = new EventEmitter<string>();

  readonly formField: any = {
    name: {
      name: "name",
      required: "Name is required"
    }
  };

  readonly homeNameForm = new FormGroup({
    [this.formField.name.name]: new FormControl("", [
      Validators.required
    ])
  });

  homeNameStatus:{
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private homeService: HomeService
  ) {}

  public onSubmit(): void {
    if (this.homeNameForm.invalid || !this.homeId) {
      return;
    }

    const newName = this.homeNameForm.get(this.formField.name.name)?.value;

    this.homeNameStatus = { loading: true };

    this.homeService.UpdateHomeName(this.homeId, newName).subscribe({
      next: () => {
        this.homeNameStatus = null;
        this.nameUpdated.emit(newName);
      },
      error: (error) => {
        this.homeNameStatus = { error: 'Failed to update home name. Please try again.' };
        console.error(error);
      }
    });
  }
}
