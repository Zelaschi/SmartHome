import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Device } from '../../../backend/services/Device/models/Device';
import { PhotosCarouselComponent } from '../../components/photos-carousel/photos-carousel.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-device-item',
  templateUrl: './device-item.component.html',
  styleUrl: './device-item.component.css',
  standalone: true,
  imports: [PhotosCarouselComponent, CommonModule]
})
export class DeviceItemComponent {
  @Input() device: Device | null = null;
  @Input() isAddingToHome: boolean = false;
  @Output() deviceAdded = new EventEmitter<Device>();

  added: boolean = false;

  addToHome(): void {
    if (this.device) {
      console.log(`Adding device ${this.device.name} to home.`);
      this.added = true;
      this.deviceAdded.emit(this.device);
    }
  }
}
