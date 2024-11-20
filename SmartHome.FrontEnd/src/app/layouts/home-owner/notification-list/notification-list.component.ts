import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { MeService } from '../../../../backend/services/Me/me.service';
import NotificationStatus from './models/NotificationStatus';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrl: './notification-list.component.css'
})
export class NotificationListComponent implements OnInit, OnDestroy {
  private _notificationSubscription: Subscription | null = null;
  loading: boolean = false;

  status: NotificationStatus = {
    notifications: [],
  };

  constructor(private readonly _meService: MeService) {}

  ngOnInit(): void {
    this.loadNotifications();
  }

  ngOnDestroy(): void {
    this._notificationSubscription?.unsubscribe();
  }

  loadNotifications(): void {
    this.loading = true;
    this._notificationSubscription = this._meService.getNotifications().subscribe({
      next: (response) => {
        console.log('Raw response:', response);
        this.status = {
          notifications: response.data || [],
          loading: false
        };
        console.log('Notifications loaded:', this.status.notifications);
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading notifications:', error);
        this.status = {
          notifications: [],
          error: 'Failed to load notifications'
        };
        this.loading = false;
      }
    });
  }
}
