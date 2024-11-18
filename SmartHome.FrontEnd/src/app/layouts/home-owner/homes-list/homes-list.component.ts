import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { MeService } from '../../../../backend/services/Me/me.service';
import HomeStatus from './models/HomeStatus';
import { Router } from '@angular/router';

@Component({
  selector: 'app-homes-list',
  templateUrl: './homes-list.component.html',
  styleUrl: './homes-list.component.css'
})
export class HomesListComponent {
  private _homeSubscription: Subscription | null = null;
  loading: boolean = false;

  constructor(
    private readonly _meService: MeService,
    private readonly _router: Router
  ) {}

  status : HomeStatus = {
    loading: true,
    homes: [],
  }

  ngOnDestroy(): void {
    this._homeSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.loadHomes();
  }

  loadHomes(): void {
    this.loading = true;
    this._meService.listAllHomesFromUser().subscribe({
      next: (response) => {
        this.status ={
          homes: response,
        }
        console.log(response);
        console.log(this.status.homes);
        this.loading = false;
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });
  }
}
