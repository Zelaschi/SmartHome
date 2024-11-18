import { Component, EventEmitter, Input, Output } from '@angular/core';
import HomeDeviceResponseModel from '../../../../backend/services/Home/models/HomeDeviceResponseModel';
import { RoomService } from '../../../../backend/services/Room/room.service';

@Component({
  selector: 'app-home-device-item',
  templateUrl: './home-device-item.component.html',
  styleUrl: './home-device-item.component.css'
})
export class HomeDeviceItemComponent {
  @Input() roomId: string | null = null;
  @Input() isAddingToRoom: boolean = false;
  @Input() homeDevice: HomeDeviceResponseModel | null = null;
  @Output() homeDeviceAdded = new EventEmitter<string>();
  showHomeDeviceNameForm: boolean = false;

  constructor(private readonly _roomService: RoomService) {}
  
  ngOnInit(): void {
    console.log(this.homeDevice?.photos);
  }

  ChangeHomeDeviceName(harwardId : string): void {
    this.showHomeDeviceNameForm = !this.showHomeDeviceNameForm;
  }

  CreateNotification(harwardId : string): void {
    console.log('CreateNotification');
  }

  onHomeDeviceNameUpdated(newName: string): void {
    this.showHomeDeviceNameForm = false;
    if (this.homeDevice) {
      this.homeDevice.name = newName;
    }
    console.log('Home name updated successfully:', newName);
  }

  addDeviceToRoom(): void {
    if (this.homeDevice?.hardwardId) {
      this.homeDeviceAdded.emit(this.homeDevice?.hardwardId ?? '');
    }
  }
}
