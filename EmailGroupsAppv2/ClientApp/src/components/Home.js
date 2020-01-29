import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Welcome</h1>
        <p>This is proposed mail groups application.</p>
        <p>In <code>v2</code> you need create an account to manage your mail groups.</p>
      </div>
    );
  }
}
