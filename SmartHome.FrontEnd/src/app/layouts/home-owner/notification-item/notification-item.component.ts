import { Component, Input } from '@angular/core';
import { DeviceNotification } from '../../../../backend/services/Me/models/DeviceNotification';

@Component({
  selector: 'app-notification-item',
  templateUrl: './notification-item.component.html',
  styleUrl: './notification-item.component.css'
})
export class NotificationItemComponent {
  @Input() notification: DeviceNotification | null = null;

  ngOnInit(): void {
    console.log(this.notification);
  }
}
