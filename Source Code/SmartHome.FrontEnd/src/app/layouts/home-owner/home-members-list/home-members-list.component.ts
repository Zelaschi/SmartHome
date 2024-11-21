import { Component, Input } from '@angular/core';
import { Subscription } from 'rxjs';
import { HomeService } from '../../../../backend/services/Home/home.service';
import HomeMemberStatus from './models/HomeMemberStatus';

@Component({
  selector: 'app-home-members-list',
  templateUrl: './home-members-list.component.html',
  styleUrl: './home-members-list.component.css'
})
export class HomeMembersListComponent {
  @Input() homeId: string | null = null;
  private _homeMembersSubscription: Subscription | null = null;
  loading: boolean = false;

  constructor(private readonly _homeService: HomeService) {}

  status : HomeMemberStatus = {
    loading: true,
    homeMembers: [],
  }

  ngOnDestroy(): void {
    this._homeMembersSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    if (this.homeId) {
      this.loadHomeMembers(this.homeId);
    }
  }

  loadHomeMembers(homeId : string) : void{
    this.loading = true;
    this._homeService.getHomeMembers(homeId).subscribe({
      next: (response) => {
        this.status ={
          homeMembers: response,
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
