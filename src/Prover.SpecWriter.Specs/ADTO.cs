using System;
using System.Runtime.Serialization;

namespace Prover.SpecWriter.Specs {
  [Serializable, DataContract(Namespace = "https://api.prover.com/Prover.SpecWriter")]
  public class ADTO {
    [DataMember(Order = 1)]
    readonly int a;
    [DataMember(Order = 2)]
    readonly int b;
    [DataMember(Order = 3)]
    readonly int c;
    [DataMember(Order = 4)]
    readonly string abc;

    public ADTO() {
    }

    public ADTO(int a, int b, int c, string abc) {
      this.a = a;
      this.b = b;
      this.c = c;
      this.abc = abc;
    }

    public int A {
      get { return a; }
    }

    public int B {
      get { return b; }
    }

    public int C {
      get { return c; }
    }

    public string Abc {
      get { return abc; }
    }

    public override string ToString() {
      return string.Format("ADTO{{B: {0}, C: {1}, Abc: {2}, A: {3}}}", B, C, Abc, A);
    }
  }
}