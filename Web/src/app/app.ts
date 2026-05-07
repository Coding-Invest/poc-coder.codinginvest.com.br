import { Component, signal, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('Web');

  constructor(private router: Router) {}

  ngOnInit(): void {
    const token = localStorage.getItem('accessToken');
    const expiry = localStorage.getItem('expiry');
    if (!token || !expiry || new Date(expiry) <= new Date()) {
      this.router.navigate(['/login']);
    } else {
      this.router.navigate(['/documentation']);
    }
  }
}