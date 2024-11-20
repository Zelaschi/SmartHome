import { Component, Input } from '@angular/core';
import RoomResponseModel from '../../../../backend/services/Room/models/RoomResponseModel';
import { RoomService } from '../../../../backend/services/Room/room.service';

@Component({
  selector: 'app-room-item',
  templateUrl: './room-item.component.html',
  styleUrl: './room-item.component.css'
})
export class RoomItemComponent {
  @Input() room: RoomResponseModel | null = null;
  @Input() homeId: string | null = null;
  added: boolean = false;
  showDevices: boolean = false;
  isAddingDeviceToRoom: boolean = false;

  constructor(private readonly _roomService: RoomService) {}

  toggleDevices(): void {
    this.showDevices = !this.showDevices;
    this.isAddingDeviceToRoom = !this.isAddingDeviceToRoom;
    console.log('Room:', this.room);
  }

  onDeviceAdded(): void {
    this.showDevices = false;
    this.isAddingDeviceToRoom = false;
    console.log('Device added succesfully');
  }

  ngOnInit(): void {
    console.log(this.room);
  }

  onHomeDeviceAdded(): void {
    this.showDevices = false;
    this.isAddingDeviceToRoom = false;
    this.added = true;
    console.log('Device added to room successfully');
  }
}
