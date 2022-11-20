import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit {
  invalidLogin!: boolean;

  loginForm = new FormGroup({
    username: new FormControl(null, [ Validators.required ]),
    password: new FormControl(null, [ Validators.required ]),
  })

  constructor(private router: Router, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit(): void {
  }
  
  login() {
    if(this.loginForm.invalid)
      return;
    
    const creadentials = {
      'username': this.loginForm.value.username,
      'password': this.loginForm.value.password
    }

    this.http.post(`${this.baseUrl}api/courses/CreateCourse`, creadentials)
      .pipe(tap(console.log))
      .subscribe({next: res  => {
        const token = (<any>res).token
        localStorage.setItem("jwt", token)
        this.invalidLogin = false
        this.router.navigate(['/'])
      }, error: err => {
        this.invalidLogin = true
        
      }})
  }
}
