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

  constructor(
      private _router: Router,
      private _http: HttpClient,
      @Inject('API_URL') private API_URL: string,
    ) { }

  ngOnInit(): void {
  }
  
  login() {
    if(this.loginForm.invalid)
      return;
  }
}
