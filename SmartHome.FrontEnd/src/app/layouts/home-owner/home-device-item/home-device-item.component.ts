import { Component, Input } from '@angular/core';
import HomeDeviceResponseModel from '../../../../backend/services/Home/models/HomeDeviceResponseModel';

@Component({
  selector: 'app-home-device-item',
  templateUrl: './home-device-item.component.html',
  styleUrl: './home-device-item.component.css'
})
export class HomeDeviceItemComponent {
  @Input() homeDevice: HomeDeviceResponseModel | null = null;
  showHomeDeviceNameForm: boolean = false;
  
  ngOnInit(): void {
    console.log(this.homeDevice);
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
}
