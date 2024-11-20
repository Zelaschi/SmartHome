import { Component, EventEmitter, Input, Output } from '@angular/core';
import HomeDeviceResponseModel from '../../../../backend/services/Home/models/HomeDeviceResponseModel';
import { RoomService } from '../../../../backend/services/Room/room.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-home-device-item',
  templateUrl: './home-device-item.component.html',
  styleUrl: './home-device-item.component.css',
})
export class HomeDeviceItemComponent {
  @Input() roomId: string | null = null;
  @Input() isAddingToRoom: boolean = false;
  @Input() homeDevice: HomeDeviceResponseModel | null = null;
  @Input() homeMemberId: string | null = null;
  @Output() homeDeviceAdded = new EventEmitter<string>();
  showHomeDeviceNameForm: boolean = false;
  successMessage: string | null = null;
  errorMessage: string | null = null;
  private _messageTimeout: any;

  constructor(
    private readonly _roomService: RoomService,
  ) {}

  ChangeHomeDeviceName(harwardId : string): void {
    this.showHomeDeviceNameForm = !this.showHomeDeviceNameForm;
  }

  private showTemporaryMessage(isSuccess: boolean, message: string) {
    if (this._messageTimeout) {
      clearTimeout(this._messageTimeout);
    }

    if (isSuccess) {
      this.successMessage = message;
      this.errorMessage = null;
    } else {
      this.errorMessage = message;
      this.successMessage = null;
    }

    this._messageTimeout = setTimeout(() => {
      this.successMessage = null;
      this.errorMessage = null;
    }, 5000);
  }

  isSecurityCamera(): boolean {
    return this.homeDevice?.type.toLowerCase() === 'security camera';
  }

  isWindowSensor(): boolean {
    return this.homeDevice?.type.toLowerCase() === 'window sensor';
  }

  onHomeDeviceNameUpdated(newName: string): void {
    this.showHomeDeviceNameForm = false;
    if (this.homeDevice) {
      this.homeDevice.name = newName;
    }
  }

  addDeviceToRoom(): void {
    if (this.roomId && this.homeDevice) {
      this._roomService.addDeviceToRoom(this.roomId, this.homeDevice.hardwardId).subscribe({
        next: () => {
          this.errorMessage = null;
          this.successMessage = 'Device added to room successfully';
        },
        error: () => {
          this.errorMessage = 'Failed to add device to room. Please try again.';
        }
      });
    }
  }

  ngOnDestroy() {
    if (this._messageTimeout) {
      clearTimeout(this._messageTimeout);
    }
  }
}
