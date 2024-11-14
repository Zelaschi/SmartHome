import { Component, Input } from '@angular/core';
import { Subscription } from 'rxjs';
import { HomeService } from '../../../../backend/services/Home/home.service';
import HomeDeviceStatus from '../home-members-list/models/HomeDeviceStatus';

@Component({
  selector: 'app-home-device-list',
  templateUrl: './home-device-list.component.html',
  styleUrl: './home-device-list.component.css'
})
export class HomeDeviceListComponent {
  @Input() homeId: string | null = null;
  private _homeDevicesSubscription: Subscription | null = null;
  loading: boolean = false;

  constructor(private readonly _homeService: HomeService) {}

  status : HomeDeviceStatus = {
    loading: true,
    homeDevices: [],
  }

  ngOnDestroy(): void {
    this._homeDevicesSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    if (this.homeId) {
      this.loadHomeDevices(this.homeId);
    }
  }

  loadHomeDevices(homeId : string) : void{
    this.loading = true;
    this._homeService.getHomeDevices(homeId).subscribe({
      next: (response) => {
        this.status ={
          homeDevices: response,
        }
        console.log(response);
        console.log(this.status.homeDevices);
        this.loading = false;
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });
  }
}
