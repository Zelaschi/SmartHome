import { Component } from '@angular/core';
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
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _roomService: RoomService
  ){}

  public onSubmit(values : RoomCreationModel) {
    this.roomStatus = { loading: true };

    this._roomService.registerRoom(values).subscribe({
      next: (response) => {
        this.roomStatus = null;

        this._router.navigate(["/devices"]);
      },
      error: (error) => {
        this.roomStatus = { error };
      },
    });
  }
}
