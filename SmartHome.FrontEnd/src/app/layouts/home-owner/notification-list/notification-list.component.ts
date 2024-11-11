import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { MeService } from '../../../../backend/services/Me/me.service';
import NotificationStatus from './models/NotificationStatus';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrl: './notification-list.component.css'
})
export class NotificationListComponent {
  private _notificationSubscription: Subscription | null = null;
  loading: boolean = false;

  constructor(private readonly _meService: MeService) {}

  status : NotificationStatus = {
    loading: true,
    notifications: [],
  }

  ngOnDestroy(): void {
    this._notificationSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.loadNotifications();
  }

  loadNotifications(): void {
    this.loading = true;
    this._meService.getNotifications().subscribe({
      next: (response) => {
        this.status ={
          notifications: response.data,
        }
        console.log(this.status.notifications);
        this.loading = false;
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });
  }
}
