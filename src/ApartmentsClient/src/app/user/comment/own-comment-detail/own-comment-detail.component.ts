import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { CommentUserService, CommentDTO } from 'src/app/services/nswag.generated.service'
import { ActivatedRoute } from '@angular/router';
import { LoggedService } from 'src/app/services/logged.service';
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-own-comment-detail',
  templateUrl: './own-comment-detail.component.html',
  styleUrls: ['./own-comment-detail.component.scss']
})
export class OwnCommentDetailComponent implements OnInit {

  comment: CommentDTO;
  isUpdating = false;

  constructor(
    private authService: LoggedService,
    private location: Location,
    private route: ActivatedRoute,
    private commentService: CommentUserService
  ) { }

  ngOnInit(): void {
    this.getComment();
  }

  get isAdmin(){
    var token = this.authService.getToken();
        
    var decodedoken = jwt_decode(token);
    var currentRole = decodedoken['role'];

    if (currentRole.includes('Admin')) {
      return true;
    }
    return false;
  }

  get isAuthor(){
    var token = this.authService.getToken();
        
    var decodedoken = jwt_decode(token);
    var currentUserId = decodedoken['id'];

    if (this.comment.authorId = currentUserId) {
      return true;
    }
    return false;
  }

  getComment(){
    const id = this.route.snapshot.paramMap.get('id');
    this.commentService.getCommentById(id)
    .subscribe(comment => this.comment = comment)
  }

  updating(){
    this.isUpdating = true;
  }

  save(): void {
    this.commentService.updateComment(this.comment)
      .subscribe(comment => {
        this.comment = comment;
        this.isUpdating = false;
    });
  }

  delete(){
    const id = this.route.snapshot.paramMap.get('id');
    this.commentService.deleteCommentById(id)
    .subscribe(()=>this.goBack());
  }

  goBack(){
    if (this.isUpdating) {
      this.isUpdating = false;
    }
    else{
      this.location.back();
    }
  }

}
