import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, Signal, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private http = inject(HttpClient);
  protected readonly title ='client';
  protected members = signal<any>([])
  async ngOnInit() {
    // this.http.get('https://localhost:5002/api/members').subscribe({
    //   next: response => this.members.set(response),
    //   error: error => console.log(error),
    //   complete: ()=> console.log("success")
    // })
    this.members.set(await this.getmember());
  }
   async getmember()
   {
     try {     
       return lastValueFrom(this.http.get('https://localhost:5002/api/members'))
     } catch (error) {
       console.log(error)
       throw error;
     }
  }
}
