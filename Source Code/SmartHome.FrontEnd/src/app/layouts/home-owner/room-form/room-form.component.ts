import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RoomService } from '../../../../backend/services/Room/room.service';
import RoomCreationModel from '../../../../backend/services/Room/models/RoomCreationModel';

@Component({
  selector: 'app-room-form',
  templateUrl: './room-form.component.html',
  styleUrl: './room-form.component.css'
})
export class RoomFormComponent {
  @Input() homeId: string | null = null;
  readonly formField: any = {
    name: {
      name: "name",
      required: "Name is required"
    }
  };
  readonly roomForm = new FormGroup({
    [this.formField.name.name]: new FormControl("", [
      Validators.required
    ])
  });

  roomStatus: {
    loading?: true;
    error?: string;
    success?: boolean;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _roomService: RoomService
  ){}

  public onSubmit(values : RoomCreationModel) {
    if (this.roomForm.invalid || !this.homeId) {
      return;
    }

    this.roomStatus = { loading: true };

    this._roomService.registerRoom(this.homeId, values).subscribe({
      next: () => {
        this.roomStatus = null;
        this.roomStatus = { success: true };
      },
      error: (error) => {
        this.roomStatus = { error };
      },
    });
  }
}
