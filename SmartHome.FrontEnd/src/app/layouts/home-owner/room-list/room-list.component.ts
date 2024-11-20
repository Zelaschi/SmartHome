import { Component, Input } from '@angular/core';
import { Subscription } from 'rxjs';
import { RoomService } from '../../../../backend/services/Room/room.service';
import RoomStatus from './models/RoomStatus';

@Component({
  selector: 'app-room-list',
  templateUrl: './room-list.component.html',
  styleUrl: './room-list.component.css'
})
export class RoomListComponent {
  @Input() homeId: string | null = null;
  private _roomSubscription: Subscription | null = null;
  loading: boolean = false;

  constructor(private readonly _roomService: RoomService) {}

  status : RoomStatus = {
    loading: true,
    rooms: [],
  }

  ngOnDestroy(): void {
    this._roomSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    if (this.homeId) {
      this.loadRooms(this.homeId);
    }
  }

  loadRooms(homeId : string) : void{
    this.loading = true;
    this._roomService.getRooms(homeId).subscribe({
      next: (response) => {
        this.status ={
          rooms: response,
        }
        this.loading = false;
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });
  }
}
