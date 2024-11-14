import { Component, Input } from '@angular/core';
import HomeDeviceResponseModel from '../../../../backend/services/Home/models/HomeDeviceResponseModel';

@Component({
  selector: 'app-home-device-item',
  templateUrl: './home-device-item.component.html',
  styleUrl: './home-device-item.component.css'
})
export class HomeDeviceItemComponent {
  @Input() homeDevice: HomeDeviceResponseModel | null = null;
  ngOnInit(): void {
    console.log(this.homeDevice);
  }
}
