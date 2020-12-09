using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP
{
    public class RGSSObject
    {
        public virtual RGSSObject clone()
        {
            return new RGSSObject();
        }
        //        Object
        //所有类的父类。它定义了对象的一般行为。

        //包含的模块Kernel 
        //方法this == other 
        //若 this 等于 other 则返回真。默认情况下，它的效果等同于 equal? 。

        //应该根据各类的具体性质来重新定义该方法。

        //this === other 
        //该方法通常用在 switch 的比较句中。默认情况下，它的运作等同于 Object#== ，但在子类中进行归属检测时，需要对其进行适当的重定义。

        //public class 
        //返回被调的类。

        //clone 
        //dup 
        //生成某对象的拷贝并返回它。clone 生成的是完整的拷贝，它包括 freeze、特殊方法等信息，而 dup 则只拷贝对象的内容。

        //请注意，clone 和 dup 生成的都是「浅层（shallow）」拷贝。它们只拷贝对象本身，而不会拷贝对象的所指（例如数组的单元等）。

        //另外，如果对拷贝进行如下测试的话

        //obj.equal?(obj.clone)

        //通常都不成立。但是

        //obj == obj.clone

        //通常都会成立。

        //若想对 true、false、null、Numeric 对象、Symbol 对象等进行拷贝时，会引发 TypeError 异常。

        //equal?(other) 
        //若 other 就是 this 自己时，即返回真。该方法不能再定义。

        //freeze 
        //禁止修改对象的内容，返回this。若强行修改则会引发 TypeError 异常。

        //frozen? 
        //若某对象禁止修改其内容则返回真。

        //inspect 
        //将某对象转为人工可读的字符串并返回该字符串。

        //instance_of?(klass) 
        //若 this 为 klass 类的直接实例时，返回真。通常，当 obj.instance_of?(c) 成立时，obj.kind_of?(c) 也是成立的。

        //is_a?(mod) 
        //kind_of?(mod) 
        //只要 this 是下列任何一个类的实例就返回真，这些类是 mod 类和它的子类，以及包括 mod 模块的类和它的子类。

        //module M
        //end
        //public class C < Object
        //  include M
        //end
        //public class S < C
        //end

        //obj = S.new
        //p obj.is_a? S       # true
        //p obj.is_a? M       # true
        //p obj.is_a? C       # true
        //p obj.is_a? Object  # true
        //p obj.is_a? Hash    # false

        //null? 
        //若被调为 null 则返回真。

        //object_id 
        //返回与各个对象对应的唯一的整数。但是，为某对象分配整数的具体情形并不固定。

        //to_ary 
        //若需要将对象隐式地转换为数组时，会在内部自动调用。

        //to_hash 
        //若需要将对象隐式地转换为哈希表时，会在内部自动调用。

        //to_int 
        //若需要将对象隐式地转换为整数时，会在内部自动调用。

        //to_s 
        //将对象转换为字符串并返回该字符串。

        //如果把非字符串对象当做参数传给 print 或 string.Format 时，将调用该方法把非字符串对象转换为字符串。

        //to_str 
        //若需要将对象隐式地转换为字符串时，会自动调用它。

        //私有方法initialize 
        //用户定义类的对象初始化方法。在对由 Class#new 新生成的对象进行初始化时要调用该方法。默认情况下，没有任何的动作。通常会在子类中根据需要对其进行再定义。传递给 Class#new 的参数会被原封不动地传递给 initialize。

    }
}
