import { Component, inject, OnInit, Signal, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { Nav } from "../Layout/nav/nav";


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Nav],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected router = inject(Router);
  // protected accountService = inject(AccountService);//login
  // private http = inject(HttpClient);
  // protected readonly title ='client';
  // protected members = signal<User[]>([])//parent to child
  // async ngOnInit() {
  //   // this.http.get('https://localhost:5002/api/members').subscribe({
  //   //   next: response => this.members.set(response),
  //   //   error: error => console.log(error),
  //   //   complete: ()=> console.log("success")
  //   // })
  //   this.members.set(await this.getmember());
  //   this.serCurrentUser();
  // }
  // serCurrentUser() {
  //   const userString = localStorage.getItem('user');
  //   if (!userString) return;
  //   const user = JSON.parse(userString)
  //   this.accountService.currentUser.set(user);
  // }
  //  async getmember()
  //  {
  //    try {     
  //      return lastValueFrom(this.http.get<User[]>('https://localhost:5002/api/members'))
  //    } catch (error) {
  //      console.log(error)
  //      throw error;
  //    }
  // }
}
