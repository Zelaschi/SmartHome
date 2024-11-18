import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';
import { HomeService } from '../../../../backend/services/Home/home.service';
import HomeDeviceStatus from '../home-members-list/models/HomeDeviceStatus';
import HomeDeviceFilters from './models/homeDeviceFilters';
import { FormControl, FormGroup } from '@angular/forms';
import { RoomService } from '../../../../backend/services/Room/room.service';

@Component({
  selector: 'app-home-device-list',
  templateUrl: './home-device-list.component.html',
  styleUrl: './home-device-list.component.css'
})
export class HomeDeviceListComponent {
  @Input() homeId: string | null = null;
  @Input() roomId: string | null = null;
  @Input() isAddingDeviceToRoom: boolean = false;
  private _homeDevicesSubscription: Subscription | null = null;
  loading: boolean = false;

  filters : HomeDeviceFilters = {
    room: null,
  }

  constructor(
    private readonly _homeService: HomeService,
    private readonly _roomService: RoomService
  ) {}

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
    this._homeService.getHomeDevices(homeId, this.filters.room).subscribe({
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

  readonly formField: any = {
    room: {
      name: 'room',
    }
  }

  readonly roomFilterForm = new FormGroup({
    [this.formField.room.name]: new FormControl('', []),
  });

  onSubmit(values: any){
    console.log('Datos del user:', values);
    this.status.loading = true;
    this.filters.room = values.room;
    if (this.homeId) {
      this.loadHomeDevices(this.homeId);
    }
  }

  onHomeDeviceAdded(homeDeviceId: string): void {
    if (this.roomId) {
      this._roomService.addDeviceToRoom(this.roomId, homeDeviceId).subscribe({
        next: () => {
          console.log('Device added to room successfully');
        }
      });
    }
  }
}
