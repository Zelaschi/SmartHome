import { Component, Input, input } from '@angular/core';
import HomeCreatedModel from '../../../../backend/services/Home/models/HomeCreatedModel';

@Component({
  selector: 'app-home-item',
  templateUrl: './home-item.component.html',
  styleUrl: './home-item.component.css'
})
export class HomeItemComponent {
  @Input() home: HomeCreatedModel | null = null;
  showMembers: boolean = false;
  showDeviceList: boolean = false;
  isAddingDevice: boolean = false;
  showHomeDevicesList: boolean = false
  showHomeNameForm: boolean = false;
  showRoomForm: boolean = false;
  showRooms: boolean = false;

  ngOnInit(): void {
    console.log(this.home);
  }

  GetHomeMembers(homeId: string): void {
    this.showMembers = !this.showMembers;
  }

  AddDeviceToHome(homeId: string): void {
    this.showDeviceList = !this.showDeviceList;
    this.isAddingDevice = !this.isAddingDevice;
  }

  GetHomeDevices(homeId: string): void {
    this.showHomeDevicesList = !this.showHomeDevicesList;
  }

  CreateRoom(homeId: string): void {
    this.showRoomForm = !this.showRoomForm;
  }

  UpdateHomeName(homeId: string): void {
    this.showHomeNameForm = !this.showHomeNameForm;
  }

  onDeviceAdded(): void {
    this.showDeviceList = false;
    this.isAddingDevice = false;
    console.log('Device added succesfully');
  }

  onNameUpdated(newName: string): void {
    this.showRoomForm = false;
    if (this.home) {
      this.home.name = newName;
    }
    console.log('Home name updated successfully:', newName);
  }

  GetRooms(homeId: string): void {
    this.showRooms = !this.showRooms;
  }
}
