import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Device } from '../../../../backend/services/Device/models/Device';

@Component({
  selector: 'app-device-item',
  templateUrl: './device-item.component.html',
  styleUrl: './device-item.component.css'
})
export class DeviceItemComponent {
  @Input() device: Device | null = null;
  @Input() isAddingToHome: boolean = false;
  @Output() deviceAdded = new EventEmitter<Device>();

  ngOnInit(): void {
    console.log(this.device);
  }

  addToHome(): void {
    if (this.device) {
      console.log(`Adding device ${this.device.name} to home.`);
      this.deviceAdded.emit(this.device);
    }
  }
}
